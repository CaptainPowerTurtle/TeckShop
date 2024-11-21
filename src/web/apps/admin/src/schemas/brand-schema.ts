import { z } from 'zod';
import {
  createSearchParamsCache,
  parseAsArrayOf,
  parseAsInteger,
  parseAsString,
  parseAsStringEnum,
} from "nuqs/server"
import { getFiltersStateParser, getSortingStateParser } from '@repo/ui/lib/parsers';

export const addBrandSchema = z.object({
  name: z.string().min(2).max(100),
  description: z.string().optional(),
  website: z.string().url().or(z.literal(''))
});
export type AddBrandSchema = z.infer<typeof addBrandSchema>

export const brandSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  description: z.string().default("").optional(),
  website: z.string().url().or(z.literal(''))
});
export type BrandSchema = z.infer<typeof brandSchema>

export const pagedBrandListSchema = z.object({
  data: z.array(brandSchema),
  page: z.coerce.number().default(1),
  size: z.coerce.number().default(10),
  totalPages: z.number(),
  totalItems: z.number(),
  isFirstPage: z.boolean(),
  isLastPage: z.boolean()
});
export type PagedBrandListSchema = z.infer<typeof pagedBrandListSchema>

// export const getBrands = z.object({
//   page: z.coerce.number().default(1),
//   size: z.coerce.number().default(15),
//   nameFilter: z.string().default("").optional(),
//   sortDecending: z.boolean().optional(),
//   sortValue: z.string().default("").optional(),
// });
// export type GetBrands = z.infer<typeof getBrands>

export const updateBrandSchema = z.object({
  // id: z.string().uuid(),
  name: z.string().optional(),
  description: z.string().optional(),
  website: z.string().optional(),
});
export type UpdateBrandSchema = z.infer<typeof updateBrandSchema>

export const deleteBrandSchema = z.object({
  id: z.string().uuid(),
});
export type DeleteBrandSchema = z.infer<typeof deleteBrandSchema>

export const deleteBrandsSchema = z.object({
  ids: z.array(z.string().uuid()),
});
export type DeleteBrandsSchema = z.infer<typeof deleteBrandsSchema>

export const brandSearchParamsCache = createSearchParamsCache({
  page: parseAsInteger.withDefault(1),
  size: parseAsInteger.withDefault(10),
  sort: getSortingStateParser<BrandSchema>().withDefault([
    { id: "name", desc: true },
  ]),
  name: parseAsString.withDefault(""),
  from: parseAsString.withDefault(""),
  to: parseAsString.withDefault(""),
  // advanced filter
  filters: getFiltersStateParser().withDefault([]),
})
export type GetBrandsSchema = Awaited<ReturnType<typeof brandSearchParamsCache.parse>>