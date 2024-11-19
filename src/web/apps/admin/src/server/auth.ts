import { DefaultJWT, JWT, JWTOptions } from "next-auth/jwt"
import KeycloakProvider from "next-auth/providers/keycloak"
import { env } from "../env"
import NextAuth, { type DefaultSession, type Profile, type User, type Session, type Account, } from "next-auth"
import authConfig from "../config/auth/auth.config";

/**
 * Module augmentation for `next-auth` types. Allows us to add custom properties to the `session`
 * object and keep type safety.
 *
 * @see https://next-auth.js.org/getting-started/typescript#module-augmentation
 */
declare module "next-auth" {
  interface Session {
    user: {
      id: string;
    } & DefaultSession["user"];
    error?: "RefreshTokenError";
    token_provider?: string;
    access_token: (string & DefaultSession) | any;
  }
  interface User {
    provider?: string;
    firstName: string | null | undefined;
    lastName?: string | null | undefined;
    isEmailVerified?: boolean | null | undefined;
    access_token: string | null;
  }
}
declare module "next-auth/jwt" {
  interface JWT {
    access_token: string;
    expires_at: number
    refresh_token: string
    id_token: string
    error?: "RefreshTokenError"
    provider?: string
    user: User
  }
}

/**
 * Options for NextAuth.js used to configure adapters, providers, callbacks, etc.
 *
 * @see https://next-auth.js.org/configuration/options
 */
export const { auth, handlers, signIn, signOut } = NextAuth({
  session: {
    strategy: "jwt",
  },
  pages: {
    signIn: '/api/signin'
  },
  callbacks: {
    async jwt({ token, account, user, profile }) {
      if (user) {
        user.email = profile?.email;
        user.firstName = profile?.given_name;
        user.lastName = profile?.family_name;
        user.isEmailVerified = profile?.email_verified;
        token.id = user.id
      }
      if (account && account?.provider === 'keycloak' && account?.type === 'oidc') {

        token.access_token = account.access_token as string,
          token.expires_at = account.expires_at as number,
          token.refresh_token = account.refresh_token as string,
          token.id_token = account.id_token as string;
        token.provider = 'keycloak'

        token.user = user;

        return token;
      } else if (Date.now() < token.expires_at * 1000) {
        return token
      } else {
        if (!token.refresh_token) throw new TypeError("Missing refresh_token")

        try {
          const url =
            `${env.KEYCLOAK_ISSUER}/protocol/openid-connect/token`
          const response = await fetch(url, {
            headers: {
              "Content-Type": "application/x-www-form-urlencoded",
            },
            body:
              new URLSearchParams({
                refresh_token: token.refresh_token ?? "",
                client_secret: env.KEYCLOAK_CLIENT_SECRET,
                grant_type: "refresh_token",
                client_id: env.KEYCLOAK_CLIENT_ID,
              }),
            method: "POST",
          })

          const tokensOrError = await response.json()

          if (!response.ok) throw tokensOrError

          const newTokens = tokensOrError as {
            access_token: string
            expires_in: number
            refresh_token?: string
          }

          token.access_token = newTokens.access_token
          token.expires_at = Math.floor(
            Date.now() / 1000 + newTokens.expires_in
          )

          if (newTokens.refresh_token)
            token.refresh_token = newTokens.refresh_token

          return token
        }
        catch (error) {
          console.error("Error refreshing access_token", error)
          // If we fail to refresh the token, return an error so we can handle it on the page
          token.error = "RefreshTokenError"
          return token
        }
      }
    },
    async session({ session, token }) {
      // if (!token.sub || typeof token.id !== "string") {
      //   return {} as Session
      // }

      if (token.provider) {
        session.token_provider = token.provider;
      }
      //session.user.id = token.sub as string

      session.error = token.error
      session.user.name = token.name;
      session.user.email = token.email ?? "";
      session.user.id = token.sub as string
      session.access_token = token.access_token;

      return session;

    },
    // async signIn({ user, credentials, account }) {
    //   return true
    // },

  },
  events: {
    signOut: async (message) => {

      const { token } = message as any

      try {
        const url =
          `${env.KEYCLOAK_ISSUER}/protocol/openid-connect/logout?` +
          new URLSearchParams({
            id_token_hint: token.id_token,
          })

        const response = await fetch(url, {
          method: "GET",
        })
        //var result = response.json()
        // The response body should contain a confirmation that the user has been logged out
      } catch (e: unknown) {
        console.error("Unable to perform post-logout handshake", e)
      }
    }
    //async signOut(message) {

    //(message?.id_token! )

  },
  ...authConfig,
  secret: env.AUTH_SECRET,
})

/**
 * Wrapper for `getServerSession` so that you don't need to import the `authOptions` in every file.
 *
 * @see https://next-auth.js.org/configuration/nextjs
 */
// export const getServerAuthSession = () => getServerSession(authOptions);
