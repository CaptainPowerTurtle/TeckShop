import {
  Breadcrumb,
  BreadcrumbList,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbSeparator,
  BreadcrumbPage,
} from "@repo/ui/components/ui/breadcrumb";
import { Card, CardContent } from "@repo/ui/components/ui/card";
import { getTranslations } from "next-intl/server";
import Link from "next/link";
import AddBrandForm  from "~/src/components/admin-panel/brands/add-brand-form";

import { ContentLayout } from "~/src/components/admin-panel/content-layout";
//import { Link } from "~/src/navigation";

export default async function BrandsPage() {
  const t = await getTranslations('rootlayout');
  return (
    <ContentLayout title="New Brand">
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
      <AddBrandForm />
    </ContentLayout>
  );
}
