import "server-only"
import { z } from "zod";
import {brandSchema, GetBrandsSchema, pagedBrandListSchema, PagedBrandListSchema} from "../../schemas/brand-schema"
import { env } from "~/src/env";
import { auth } from "~/src/server/auth";
import { getLocale } from "next-intl/server";
import { redirect } from "~/src/navigation";

export async function getBrandsQuery(input: GetBrandsSchema) {

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
        const response = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/brands?page=${input.page}&size=${input.size}&nameFilter=${input.name}&sortDecending=${sortDecending}&sortValue=${sortValue}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${session?.access_token}`
            },
        });

        if (!response.ok) {
            if (response.status == 401) {
                redirect(`/api/signin?callbackUrl=${encodeURIComponent(`/${await getLocale()}/brands`)}`)
            }
            throw new Error(response.statusText);

        }
        const brands: PagedBrandListSchema = await response.json()
        return {data: brands.data, totalPages: brands.totalPages, totalItems: brands.totalItems}
    }
    catch(error) {
        return { data: [], totalPages: 0 };
    }
    // const brandResponse: PagedBrandListSchema = await fetch(`${env.TECKNET_BACKEND_API_URL}/catalog/api/brands?pageNumber=${input.pageNumber}&pageSize=${input.pageSize}`, {
    //     method: 'GET',
    //     headers: {
    //         'Authorization': `Bearer ${session?.access_token}`
    //     },
    // })
    // .then((async response => pagedBrandListSchema.parse(await response.json())))
    // .catch((error) => {error});
    
    // const response = await fetch(`${env.TECKNET_BACKEND_API_URL}/catalog/api/brands?pageNumber=${input.pageNumber}&pageSize=${input.pageSize}`, {
    //     method: 'GET',
    //     headers: {
    //         'Authorization': `Bearer ${session?.access_token}`
    //     },
    // });

    // if (!response.ok) {
    //     //Return error
    //     return;
    // }

    // const pagedBrandList: 
}