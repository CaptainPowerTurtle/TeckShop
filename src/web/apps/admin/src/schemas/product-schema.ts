import { z } from "zod";
import { brandSchema } from "./brand-schema";

export const productSchema = z.object({
    id: z.string().uuid(),
    name: z.string(),
    description: z.string().default("").optional(),
    slug: z.string(),
    productSku: z.string().default("").optional(),
    gtin: z.string().default("").optional(),
    isActive: z.boolean(),
    brand: z.object({brandSchema}).optional()
  });
  export type ProductSchema = z.infer<typeof productSchema>