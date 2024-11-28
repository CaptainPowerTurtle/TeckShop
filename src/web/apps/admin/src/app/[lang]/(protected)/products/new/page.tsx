import { Breadcrumb, BreadcrumbItem, BreadcrumbLink, BreadcrumbList, BreadcrumbPage, BreadcrumbSeparator } from "@teckshop/ui/components/ui/breadcrumb";
import { Card, CardContent } from "@teckshop/ui/components/ui/card";
import { cn } from "@teckshop/ui/lib/utils";
import { getTranslations } from "next-intl/server";
import AddBrandForm from "~/src/components/admin-panel/brands/add-brand-form";
import { ContentLayout } from "~/src/components/admin-panel/content-layout";
import AddProductForm from "~/src/components/admin-panel/products/new-product";
import { env } from "~/src/env";
import { Link } from "~/src/navigation";
import { brandSchema, brandsList } from "~/src/schemas/brand-schema";
import { auth } from "~/src/server/auth";

export default async function ProductsPage() {
    const t = await getTranslations('rootlayout');
    const session = await auth();
    var res = await fetch(`${env.TECKSHOP_BACKEND_CATALOG_ROUTE_V1}/brands/all`, {
      headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${session?.access_token}`
      },
  });
  const brands = await brandsList.parseAsync(await res.json())
    return (
      <ContentLayout title="New Product">
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
                      <BreadcrumbLink asChild>
                        <Link href="/brands">{t('brand.brands')}</Link>
                      </BreadcrumbLink>
                    </BreadcrumbItem>
                    <BreadcrumbSeparator />
                    <BreadcrumbItem>
                      <BreadcrumbPage>{t('new')}</BreadcrumbPage>
                    </BreadcrumbItem>
                  </BreadcrumbList>
                </Breadcrumb>
              </div>
            </CardContent>
          </Card>
        </div>
        <div className={cn("grid grid-cols-3 gap-4")}>
          <div className={cn("col-span-2")}>
          <AddProductForm brands={brands}/>
          </div>
          <div className={cn("col-span-1")}>
          <AddBrandForm />
          </div>
        </div>
      </ContentLayout>
    );
  }
  