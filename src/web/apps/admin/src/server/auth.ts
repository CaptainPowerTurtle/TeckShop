import { DefaultJWT, JWT, JWTOptions } from "next-auth/jwt"
import KeycloakProvider from "next-auth/providers/keycloak"
import { env } from "../env"
import NextAuth, { type DefaultSession, type Profile, type User, type Session, type Account, } from "next-auth"
import authConfig from "../config/auth/auth.config";
import { ProviderType } from "next-auth/providers/index";

/**
 * Module augmentation for `next-auth` types. Allows us to add custom properties to the `session`
 * object and keep type safety.
 *
 * @see https://next-auth.js.org/getting-started/typescript#module-augmentation
 */
declare module "next-auth" {
  interface Session {
    user: {
      sub: string;
      email_verified: boolean;
      name: string;
      preferred_username: string;
      given_name: string;
      family_name: string;
      email: string;
      id: string;
      org_name?: string;
      telephone?: string;
    } & DefaultSession["user"];
    error?: string | null;
    token_provider?: string;
    access_token: (string & DefaultSession) | any;
  }
  interface User {
    sub: string;
    email_verified: boolean;
    name?: string | null | undefined;
    telephone: string;
    preferred_username: string;
    org_name: string;
    given_name: string;
    family_name: string;
    email?: string | null | undefined;
    id?: string | undefined;
    organization: any;
  }

  interface Account {
    provider: string;
    type: ProviderType;
    id: string;
    access_token: string;
    refresh_token: string;
    idToken: string;
    expires_in: number;
    refresh_expires_in: number;
    token_type: string;
    id_token: string;
    "not-before-policy": number;
    session_state: string;
    scope: string;
  }

  interface Profile {
    sub?: string | null;
    email_verified?: boolean | null;
    name?: string | null;
    telephone: string;
    preferred_username?: string | null;
    org_name: string;
    given_name?: string | null;
    family_name?: string | null;
    email?: string | null;
  }
}
declare module "next-auth/jwt" {
  interface JWT {
    access_token: string;
    refresh_token: string;
    refresh_expires_in: number;
    expires_in: number;
    user: User
    error?: string | null;
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
      if (account && user) {
        // Update token with account information
        token.access_token = account.access_token;
        token.refresh_token = account.refresh_token;
        token.id_token = account.id_token as string;
        token.access_token_expired =
          Date.now() + (account.expires_in - 15) * 1000;
        token.refresh_token_expired =
          Date.now() + (account.refresh_expires_in - 15) * 1000;
        token.user = user;
        return token;
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

          return {
            ...token,
            access_token: tokensOrError.access_token,
            refresh_token: tokensOrError.refresh_token ?? token.refresh_token,
            refresh_token_expired:
            tokensOrError.refresh_expires_in ?? token.refresh_token_expired,
            expires_in: Math.floor(Date.now() / 1000 + tokensOrError.expires_in),
            error: null,
          };

          // const newTokens = tokensOrError as {
          //   access_token: string
          //   expires_in: number
          //   refresh_token?: string
          // }

          // token.access_token = newTokens.access_token
          // token.expires_at = Math.floor(
          //   Date.now() / 1000 + newTokens.expires_in
          // )

          // if (newTokens.refresh_token)
          //   token.refresh_token = newTokens.refresh_token

          // return token
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
      session.error = token.error
      session.user.name = token.name ?? "";
      session.user.email = token.email ?? "";
      session.user.id = token.user.sub as string
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
