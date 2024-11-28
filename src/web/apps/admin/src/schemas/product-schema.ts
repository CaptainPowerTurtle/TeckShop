import { z } from 'zod';
import {
  createSearchParamsCache,
  parseAsArrayOf,
  parseAsInteger,
  parseAsString,
  parseAsStringEnum,
} from "nuqs/server"
import { getFiltersStateParser, getSortingStateParser } from '@teckshop/ui/lib/parsers';
import { brandSchema } from './brand-schema';

export const addProductSchema = z.object({
  name: z.string().min(1).max(255),
  description: z.string().optional(),
  isActive: z.boolean(),
  productSKU: z.string().default("").optional(),
  gtin: z.string().default("").optional(),
  brandId: z.string().uuid().nullable(),
});
export type AddProduct = z.infer<typeof addProductSchema>

export const productSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  description: z.string().default("").optional(),
  slug: z.string(),
  isActive: z.boolean(),
  productSKU: z.string().default("").optional(),
  gtin: z.string().default("").optional(),
  brand: brandSchema.nullable()
});
export type Product = z.infer<typeof productSchema>

export const pagedProductsListSchema = z.object({
  data: z.array(productSchema),
  page: z.coerce.number().default(1),
  size: z.coerce.number().default(10),
  totalPages: z.number(),
  totalItems: z.number(),
  isFirstPage: z.boolean(),
  isLastPage: z.boolean()
});
export type PagedProductsList = z.infer<typeof pagedProductsListSchema>

export const updateProductSchema = z.object({
  // id: z.string().uuid(),
  name: z.string().max(255),
  description: z.string().optional(),
  isActive: z.boolean(),
  productSKU: z.string().optional(),
  gtin: z.string().optional(),
});
export type UpdateProduct = z.infer<typeof updateProductSchema>

export const deleteProductSchema = z.object({
  id: z.string().uuid(),
});
export type DeleteProduct = z.infer<typeof deleteProductSchema>

export const deleteProductsSchema = z.object({
  ids: z.array(z.string().uuid()),
});
export type DeleteProducts = z.infer<typeof deleteProductsSchema>

export const productsSearchParamsCache = createSearchParamsCache({
  page: parseAsInteger.withDefault(1),
  size: parseAsInteger.withDefault(10),
  sort: getSortingStateParser<Product>().withDefault([
    { id: "name", desc: true },
  ]),
  name: parseAsString.withDefault(""),
  from: parseAsString.withDefault(""),
  to: parseAsString.withDefault(""),
  // advanced filter
  filters: getFiltersStateParser().withDefault([]),
})
export type GetProducts = Awaited<ReturnType<typeof productsSearchParamsCache.parse>>