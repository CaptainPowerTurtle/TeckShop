import { Breadcrumb, BreadcrumbList, BreadcrumbItem, BreadcrumbLink, BreadcrumbSeparator, BreadcrumbPage } from "@repo/ui/components/ui/breadcrumb";
import { Card, CardContent } from "@repo/ui/components/ui/card";
import { getTranslations } from "next-intl/server";
import { ContentLayout } from "~/src/components/admin-panel/content-layout";
import { env } from "~/src/env";
import { Link } from "~/src/navigation";
import { auth } from "~/src/server/auth";
import { brandSchema, getBrands, pagedBrandListSchema, searchParamsCache } from "~/src/schemas/brand-schema";
import { z } from "zod";
import { BrandsTable } from "~/src/components/admin-panel/brands/data-table/brands-table";
import { getBrandsQuery } from "~/src/lib/data/brand-queries";
import { SearchParams } from "@repo/ui/types/index";



interface BrandsPageProps {
  searchParams: Promise<SearchParams>
}

export default async function BrandsPage(props: BrandsPageProps) {

  const session = await auth();
  if (session == null || session == undefined) {
      return;
  }

  const searchParams = await props.searchParams
  const search = searchParamsCache.parse(searchParams)

  const promises = Promise.all([
    getBrandsQuery(search)
  ])
//   type data = z.infer<typeof brandSchema>;
//   const brandResponse: z.infer<typeof pagedBrandListSchema> = await fetch(`${env.TECKNET_BACKEND_API_URL}/catalog/api/brands?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
//     method: 'GET',
//     headers: {
//         'Authorization': `Bearer ${session?.access_token}`
//     },
// }).then((async response => pagedBrandListSchema.parse(await response.json())));
// var test = await fetch(`${env.TECKNET_BACKEND_API_URL}/catalog/api/brands?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
//   method: 'GET',
//   headers: {
//       'Authorization': `Bearer ${session?.access_token}`
//   },
// });

// var test2 = await test.json();
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
                    <BreadcrumbPage>{t('brand.all-brands')}</BreadcrumbPage>
                  </BreadcrumbItem>
                </BreadcrumbList>
              </Breadcrumb>
            </div>
          </CardContent>
        </Card>
      </div>
      <BrandsTable promises={promises} />
    </ContentLayout>
    );
  }
  