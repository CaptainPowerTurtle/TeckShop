'use server';
import { env } from "../../env"

import { actionClient } from "~/src/lib/safe-action";
import { flattenValidationErrors } from "next-safe-action";
import { auth } from "~/src/server/auth";
import { addBrandSchema, deleteBrandSchema, deleteBrandsSchema, updateBrandSchema } from "~/src/schemas/brand-schema";
import { revalidatePath } from "next/cache";
import { z } from "zod";

export const addBrandAction = actionClient
    .schema(addBrandSchema, {
        handleValidationErrorsShape: (ve) => flattenValidationErrors(ve).fieldErrors,
    })
    .action(async ({ parsedInput: { name, description, website } }) => {

        // Check valid login here
        //console.log(`${env.TECKNET_BACKEND_API_URL}/catalog/api/brands`)
        const session = await auth();
        if (session == null || session == undefined) {
            return;
        }
        const body = JSON.stringify({
            name: name,
            description: description,
            website: website,
        });

        const crypto = require('crypto');
        const idempotencyKey = crypto.createHash('sha256').update(body).digest('hex');
        var res = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/brands`, {
            method: 'POST',
            headers: {
                'Idempotency-Key': idempotencyKey,
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${session?.access_token}`
            },
            body: body,
        });
        if (!res.ok) {
            return { error: res.statusText }
        }
        return { message: "Brand created!" }
    });

export const updateBrandAction = actionClient
    .schema(updateBrandSchema, {
        handleValidationErrorsShape: (ve) => flattenValidationErrors(ve).fieldErrors,
    })
    .schema(async (prevSchema) => {
        return prevSchema.extend({id: z.string().uuid()})
    })
    .action(async ({ parsedInput: { id, name, description, website } }) => {

        // Check valid login here
        //console.log(`${env.TECKNET_BACKEND_API_URL}/catalog/api/brands`)
        const session = await auth();
        if (session == null || session == undefined) {
            return { error: "Session was not found" }
        }
        const body = JSON.stringify({
            id: id,
            name: name,
            description: description,
            website: website,
        });
        try {


            var res = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/brands`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${session?.access_token}`
                },
                body: body,
            });
            if (!res.ok) {
                return { error: res.statusText }
            }

            revalidatePath("/")
            
            return { message: "Brand updated!" }

        } catch (error) {

            return { error: "Something went wrong" }
        }
    });
export const deleteBrandAction = actionClient
    .schema(deleteBrandSchema, {
        handleValidationErrorsShape: (ve) => flattenValidationErrors(ve).fieldErrors,
    })
    .action(async ({ parsedInput: { id } }) => {

        // Check valid login here
        //console.log(`${env.TECKNET_BACKEND_API_URL}/catalog/api/brands`)
        const session = await auth();
        if (session == null || session == undefined) {
            return { error: "Session was not found" }
        }
        try {
            
            var res = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/brands/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${session?.access_token}`
                },
            });
            if (!res.ok) {
                return { error: res.statusText }
            }
            return { message: "Brand deleted!" }

        } catch (error) {

            return { error: "Something went wrong" }
        }
    });
    export const deleteBrandsAction = actionClient
    .schema(deleteBrandsSchema, {
        handleValidationErrorsShape: (ve) => flattenValidationErrors(ve).fieldErrors,
    })
    .action(async ({ parsedInput}) => {

        // Check valid login here
        //console.log(`${env.TECKNET_BACKEND_API_URL}/catalog/api/brands`)
        const session = await auth();
        
        if (session == null || session == undefined) {
            return { error: "Session was not found" }
        }
        try {
            const body = JSON.stringify({
                ids: parsedInput.ids,
            });
            var res = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/brands/bulk/delete`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${session?.access_token}`,
                    'Content-Type': 'application/json',
                },
                body: body
            });

            if (!res.ok) {
                return { error: res.statusText }
            }
            revalidatePath("/")
            return { message: "Brands deleted!" }

        } catch (error) {

            return { error: "Something went wrong" }
        }
    });