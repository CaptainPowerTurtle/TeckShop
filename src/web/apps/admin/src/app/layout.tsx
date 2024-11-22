import { ReactNode } from "react";
import { Metadata } from "next";
import { getMessages, getTranslations } from "next-intl/server";
import { NextIntlClientProvider } from "next-intl";
import { Inter as FontSans } from "next/font/google";
import { cn } from "@teckshop/ui/lib/utils";
import "@teckshop/ui/globals.css";

export const metadata: Metadata = {
  title: "Home",
  description: "Welcome to Next.js boilerplate",
};
// export async function generateMetadata({
//   params: { locale },
// }: {
//   params: { locale: string };
// }) {
//   const t = await getTranslations({ locale, namespace: 'app' });

//   return {
//     title: t("name"),
//     description: t("description"),
//   };
// }


export default async function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return children;
}