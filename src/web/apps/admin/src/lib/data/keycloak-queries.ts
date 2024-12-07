import { getLocale } from "next-intl/server";
import { env } from "~/src/env";
import { redirect } from "~/src/navigation";
import { GetUserOrganization, OrganizationList } from "~/src/schemas/keycloak-schema";
import { auth } from "~/src/server/auth";

export async function getUserOrganizations() : Promise<OrganizationList> {

    const session = await auth();

    try {
        const response = await fetch(`${env.KEYCLOAK_ADMIN}/organizations/members/${session?.user.id}/organizations`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${session?.access_token}`
            },
        });
        console.log(response)
        if (!response.ok) {
            if (response.status == 401) {
                redirect(`/api/signin?callbackUrl=${encodeURIComponent(`/${await getLocale()}/brands`)}`)
            }
            throw new Error(response.statusText);

        }
        
        const organizations: OrganizationList = await response.json()
        console.log(organizations)
        return organizations
    }
    catch(error) {
        return []
    }
}