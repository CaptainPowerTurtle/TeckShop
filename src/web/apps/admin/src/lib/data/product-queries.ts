import "server-only"
import { z } from "zod";
import { env } from "~/src/env";
import { auth } from "~/src/server/auth";
import { getLocale } from "next-intl/server";
import { redirect } from "~/src/navigation";
import { GetProducts, PagedProductsList } from "~/src/schemas/product-schema";

export async function getPagedProductsQuery(input: GetProducts) {

    const session = await auth();
    var sortValue = "";
    var sortDecending = true;

    input.sort.length > 0
      ? input.sort.map((item) =>
      {
        sortDecending = item.desc,
        sortValue = item.id
      }
        )
      : sortValue = "createdOn", sortDecending, sortDecending = true

    try {
        const response = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/products?page=${input.page}&size=${input.size}&nameFilter=${input.name}&sortDecending=${sortDecending}&sortValue=${sortValue}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${session?.access_token}`
            },
        });

        if (!response.ok) {
            if (response.status == 401) {
                redirect(`/api/signin?callbackUrl=${encodeURIComponent(`/${await getLocale()}/products`)}`)
            }
            throw new Error(response.statusText);

        }
        const products: PagedProductsList = await response.json()
        return {data: products.data, totalPages: products.totalPages, totalItems: products.totalItems}
    }
    catch(error) {
        return { data: [], totalPages: 0 };
    }
}