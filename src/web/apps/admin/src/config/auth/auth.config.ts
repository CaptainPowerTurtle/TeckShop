import type { NextAuthConfig } from "next-auth"
import keycloak from "next-auth/providers/keycloak"
import { env } from "~/src/env"
 
export default { secret: env.AUTH_SECRET , providers: [keycloak({
    clientId: env.KEYCLOAK_CLIENT_ID,
    clientSecret: env.KEYCLOAK_CLIENT_SECRET,
    issuer: env.KEYCLOAK_ISSUER,
    authorization: {
      params: {
        audience: 'catalog',
      }
    }
  })] } satisfies NextAuthConfig