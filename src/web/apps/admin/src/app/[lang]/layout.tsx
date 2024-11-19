import React from "react";
import RootProviders from "../../config/providers/providers";
import { NextIntlClientProvider } from "next-intl";
import { getMessages } from "next-intl/server";
import { Inter as FontSans } from "next/font/google";
import { cn } from "@repo/ui/lib/utils";
import "@repo/ui/globals.css";
import { Toaster } from "@repo/ui/components/ui/sonner";
const fontSans = FontSans({
  subsets: ["latin"],
  variable: "--font-sans",
});

export default async function RootLayout({
  children,
  params: { locale },
}: {
  children: React.ReactNode;
  params: { locale: string };
}) {
  // Providing all messages to the client
  // side is the easiest way to get started
  //const messages = await getMessages();
  const messages = await getMessages();
  return (
    <html lang={locale} suppressHydrationWarning>
      <body
        className={cn(
          "min-h-screen bg-background font-sans antialiased",
          fontSans.variable,
        )}
      >
        <NextIntlClientProvider messages={messages}>
          <RootProviders>
            
            {children}
            <Toaster />
            </RootProviders>
        </NextIntlClientProvider>
      </body>
    </html>
  );
}
