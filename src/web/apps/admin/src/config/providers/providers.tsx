import { ReactNode } from "react";
import { NextAuthProvider } from "./auth-provider";
import { ThemeProvider } from "./theme-provider";
import React from "react";

export default async function RootProviders({ children }: { children: ReactNode; }) {
  
    return (
      <NextAuthProvider>
      <ThemeProvider attribute="class" defaultTheme="system" enableSystem>
      {children}
      </ThemeProvider>
    </NextAuthProvider>
    )
  }
  