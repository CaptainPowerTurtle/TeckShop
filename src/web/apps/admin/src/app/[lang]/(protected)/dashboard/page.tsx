
import { Breadcrumb, BreadcrumbItem, BreadcrumbLink, BreadcrumbList, BreadcrumbPage, BreadcrumbSeparator } from "@repo/ui/components/ui/breadcrumb";
import { Card, CardContent } from "@repo/ui/components/ui/card";
import { getTranslations } from "next-intl/server";
import Link from "next/link";
import React from "react";
import { ContentLayout } from "~/src/components/admin-panel/content-layout";

export default async function DashboardPage() {
  const t = await getTranslations('rootlayout');
  return (
    <ContentLayout title="Dashboard">
      <div className="grid grid-flow-col auto-cols-max">
        <Card>
          <CardContent className="content-center py-2">
            <div>
              <Breadcrumb>
                <BreadcrumbList>
                <BreadcrumbItem>
                    <BreadcrumbPage>{t('dashboard')}</BreadcrumbPage>
                  </BreadcrumbItem>
                </BreadcrumbList>
              </Breadcrumb>
            </div>
          </CardContent>
        </Card>
      </div>
    </ContentLayout>
  );
}
