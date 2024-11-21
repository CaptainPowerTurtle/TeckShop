'use server';
import { env } from "../../env"

import { actionClient } from "~/src/lib/safe-action";
import { flattenValidationErrors } from "next-safe-action";
import { auth } from "~/src/server/auth";
import { revalidatePath } from "next/cache";
import { z } from "zod";
import { addProductSchema, deleteProductSchema, deleteProductsSchema, updateProductSchema } from "~/src/schemas/product-schema";

export const addProductAction = actionClient
    .schema(addProductSchema, {
        handleValidationErrorsShape: (ve) => flattenValidationErrors(ve).fieldErrors,
    })
    .action(async ({ parsedInput: { name, description, isActive, productSKU, gtin } }) => {

        // Check valid login here
        const session = await auth();
        if (session == null || session == undefined) {
            return;
        }
        const body = JSON.stringify({
            name: name,
            description: description,
            isActive: isActive,
            productSKU: productSKU,
            gtin: gtin
        });

        const crypto = require('crypto');
        const idempotencyKey = crypto.createHash('sha256').update(body).digest('hex');
        var res = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/products`, {
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
        return { message: "Product created!" }
    });

export const updateProductAction = actionClient
    .schema(updateProductSchema, {
        handleValidationErrorsShape: (ve) => flattenValidationErrors(ve).fieldErrors,
    })
    .schema(async (prevSchema) => {
        return prevSchema.extend({id: z.string().uuid()})
    })
    .action(async ({ parsedInput: { id, name, description, isActive, productSKU, gtin } }) => {

        // Check valid login here
        const session = await auth();
        if (session == null || session == undefined) {
            return { error: "Session was not found" }
        }
        const body = JSON.stringify({
            id: id,
            name: name,
            description: description,
            isActive: isActive,
            productSKU: productSKU,
            gtin: gtin
        });
        try {


            var res = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/products`, {
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
            
            return { message: "Product updated!" }

        } catch (error) {

            return { error: "Something went wrong" }
        }
    });
export const deleteProductAction = actionClient
    .schema(deleteProductSchema, {
        handleValidationErrorsShape: (ve) => flattenValidationErrors(ve).fieldErrors,
    })
    .action(async ({ parsedInput: { id } }) => {

        // Check valid login here
        const session = await auth();
        if (session == null || session == undefined) {
            return { error: "Session was not found" }
        }
        try {
            
            var res = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/products/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${session?.access_token}`
                },
            });
            if (!res.ok) {
                return { error: res.statusText }
            }
            return { message: "Product deleted!" }

        } catch (error) {

            return { error: "Something went wrong" }
        }
    });
    export const deleteProductsAction = actionClient
    .schema(deleteProductsSchema, {
        handleValidationErrorsShape: (ve) => flattenValidationErrors(ve).fieldErrors,
    })
    .action(async ({ parsedInput}) => {

        // Check valid login here
        const session = await auth();
        
        if (session == null || session == undefined) {
            return { error: "Session was not found" }
        }
        try {
            const body = JSON.stringify({
                ids: parsedInput.ids,
            });
            var res = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/products/bulk/delete`, {
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
            return { message: "Products deleted!" }

        } catch (error) {

            return { error: "Something went wrong" }
        }
    });