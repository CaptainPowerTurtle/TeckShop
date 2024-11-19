import "server-only"
import { z } from "zod";
import {GetBrands, pagedBrandListSchema, PagedBrandListSchema} from "../../schemas/brand-schema"
import { env } from "~/src/env";
import { auth } from "~/src/server/auth";

export async function getBrandsQuery(input: GetBrands) {

    const session = await auth();
    
    try {
        const response = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/brands?page=${input.page}&size=${input.size}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${session?.access_token}`
            },
        });

        if (!response.ok) {

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