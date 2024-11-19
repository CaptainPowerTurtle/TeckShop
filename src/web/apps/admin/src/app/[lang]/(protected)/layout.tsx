import React from "react";
import AdminPanelLayout from "~/src/components/admin-panel/admin-panel-layout";
import { auth, signIn } from "~/src/server/auth";
import { redirect } from "next/navigation";
import { getLocale } from "next-intl/server";


export default async function ProtectedLayout({
  children,
}: {
  children: React.ReactNode;
}) {

  const session = await auth()
  if (session?.error === "RefreshTokenError") {
    redirect(`/api/signin?callbackUrl=${encodeURIComponent(`/${await getLocale()}/dashboard`)}`)
  }
  return (
    <div className=" h-full w-full -z-10">
    <AdminPanelLayout>
        <div className="absolute -z-20 bottom-0 left-0 right-0 top-0 bg-[linear-gradient(to_right,#4f4f4f2e_1px,transparent_1px),linear-gradient(to_bottom,#4f4f4f2e_1px,transparent_1px)] bg-[size:14px_24px] [mask-image:radial-gradient(ellipse_60%_50%_at_50%_0%,#000_70%,transparent_100%)]">
        </div>
      {children}
    </AdminPanelLayout>
    </div>
  );
}
