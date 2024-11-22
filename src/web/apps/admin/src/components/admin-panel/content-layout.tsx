import React from "react";
import { Navbar } from "./navbar";
import { cn } from "@teckshop/ui/lib/utils";

interface ContentLayoutProps {
  title: string;
  children: React.ReactNode;
}

export function ContentLayout({ title, children }: ContentLayoutProps) {
  return (
    <div>
            {/* <Navbar
        user={{ name: session?.user.name, email: session?.user.email }}
        title="dasda"
        translation={{
          dashboard: t("account"),
          profile: t("profile"),
          signOut: t("signOut"),
          account: t("account"),
        }}
      /> */}
      <Navbar title={title} />
      <div className="container pt-8 pb-8 px-4 ">{children}</div>
    </div>
  );
}