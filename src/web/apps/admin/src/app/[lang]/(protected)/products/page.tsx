import { Breadcrumb, BreadcrumbList, BreadcrumbItem, BreadcrumbLink, BreadcrumbSeparator, BreadcrumbPage } from "@repo/ui/components/ui/breadcrumb";
import { Card, CardContent } from "@repo/ui/components/ui/card";
import { getTranslations } from "next-intl/server";
import { ContentLayout } from "~/src/components/admin-panel/content-layout";
import { env } from "~/src/env";
import { Link } from "~/src/navigation";
import { auth } from "~/src/server/auth";
import { brandSearchParamsCache } from "~/src/schemas/brand-schema";
import { z } from "zod";
import { BrandsTable } from "~/src/components/admin-panel/brands/data-table/brands-table";
import { getBrandsQuery } from "~/src/lib/data/brand-queries";
import { SearchParams } from "@repo/ui/types/index";
import React from "react";
import { DataTableSkeleton } from "@repo/ui/components/data-table/data-table-skeleton"
import { getPagedProductsQuery } from "~/src/lib/data/product-queries";
import { productsSearchParamsCache } from "~/src/schemas/product-schema";
import { ProductsTable } from "~/src/components/admin-panel/products/data-table/products-table";


interface ProductsPageProps {
  searchParams: Promise<SearchParams>
}

export default async function ProductsPage(props: ProductsPageProps) {

  const session = await auth();
  if (session == null || session == undefined) {
      return;
  }

  const searchParams = await props.searchParams
  const search = productsSearchParamsCache.parse(searchParams)

  const promises = Promise.all([
    getPagedProductsQuery(search)
  ])
  const t = await getTranslations('rootlayout');
    return (
      <ContentLayout title="All Brands">
      <div className="grid grid-flow-col auto-cols-max pb-8">
        <Card>
          <CardContent className="content-center py-2">
            <div>
              <Breadcrumb>
                <BreadcrumbList>
                  <BreadcrumbItem>
                    <BreadcrumbLink asChild>
                      <Link href="/dashboard">{t('dashboard')}</Link>
                    </BreadcrumbLink>
                  </BreadcrumbItem>
                  <BreadcrumbSeparator />
                  <BreadcrumbItem>
                    <BreadcrumbPage>{t('product.all-products')}</BreadcrumbPage>
                  </BreadcrumbItem>
                </BreadcrumbList>
              </Breadcrumb>
            </div>
          </CardContent>
        </Card>
      </div>
      <React.Suspense
          fallback={
            <DataTableSkeleton
              columnCount={5}
              searchableColumnCount={1}
              filterableColumnCount={2}
              cellWidths={["10rem", "40rem", "12rem", "12rem", "8rem",]}
              shrinkZero
            />
          }
        >
          <ProductsTable promises={promises} />
        </React.Suspense>
    </ContentLayout>
    );
  }
  