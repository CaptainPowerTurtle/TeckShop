import { signIn } from "next-auth/react"

export async function signOut(provider?: string) {

    provider != null ? provider : 'keycloak'

    await signIn(provider)

}