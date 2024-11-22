"use client";


import { useStore } from "@teckshop/ui/hooks/use-store"
import { Footer } from "./footer";
import { Sidebar } from "./sidebar";
import React, { useEffect } from "react";
import { Navbar } from "./navbar";
import { useSidebarToggle } from "~/src/hooks/use-sidebar-toggle";
import { cn } from "@teckshop/ui/lib/utils";
import { Session } from "next-auth";
import { signIn } from "~/src/server/auth";
import { usePathname } from "~/src/navigation";

export default function AdminPanelLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const sidebar = useStore(useSidebarToggle, (state) => state);

  if (!sidebar) return null;

  // useEffect(() => {
  //   if (session?.error !== "RefreshTokenError") return
  //   const path = usePathname();
  //   signIn({callbackUrl: path}) // Force sign in to obtain a new set of access and refresh tokens
  // }, [session?.error])
  
  return (
    <>
      <Sidebar />
      {/* <main
        className={cn(
          "min-h-[calc(100vh_-_56px)] bg-zinc-50 dark:bg-slate-950 transition-[margin-left] ease-in-out duration-300",
          sidebar?.isOpen === false ? "lg:ml-[90px]" : "lg:ml-72"
        )}
      > */}
            <main
        className={cn(
          "min-h-[calc(100vh_-_56px)] transition-[margin-left] ease-in-out duration-300 z-20",
          sidebar?.isOpen === false ? "lg:ml-[90px]" : "lg:ml-72"
        )}
      >
        {children}
      </main>
      <footer
        className={cn(
          "transition-[margin-left] ease-in-out duration-300",
          sidebar?.isOpen === false ? "lg:ml-[90px]" : "lg:ml-72"
        )}
      >
        <Footer />
      </footer>
    </>
  );
}