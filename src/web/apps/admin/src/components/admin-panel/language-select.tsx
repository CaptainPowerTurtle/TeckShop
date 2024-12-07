"use client";

import { Button } from "@teckshop/ui/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@teckshop/ui/components/ui/dropdown-menu";
import { useLocale } from "next-intl";
import Image from "next/image";
import { useParams, useSearchParams } from "next/navigation";

import { ChangeEvent, useTransition } from "react";
import { defaultLocale, locales } from "~/src/config/i18n/config";
import { usePathname, useRouter } from "~/src/navigation";
//import { usePathname, useRouter } from "~/src/navigation";

export function LanguageSelect() {
  const router = useRouter();
  const [isPending, startTransition] = useTransition();
  const pathname = usePathname();
  const locale = useLocale();

  function setSelectedLanguage(locale: string) {
    startTransition(() => {
      router.replace(pathname, {locale: locale}
      );
      // router.replace(
      //   // @ts-expect-error -- TypeScript will validate that only known `params`
      //   // are used in combination with a given `pathname`. Since the two will
      //   // always match for the current route, we can skip runtime checks.
      //   { pathname, params },
      //   { locale: localetest },
      // );
    });
  }
  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild disabled={isPending}>
        <Button variant="outline" className="flex items-center gap-2">
          <Image
            src={locales.find((lang) => lang.code === locale)?.flag!}
            alt={
              locales.find((lang) => lang.code === locale)?.name || "English"
            }
            width={24}
            height={24}
            className="rounded-full"
            style={{ aspectRatio: "24/24", objectFit: "cover" }}
          />
          {locales.find((lang) => lang.code === locale)?.name || "English"}
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end" className="w-[200px]">
        {locales.map((lang) => (
          <DropdownMenuItem
            defaultValue={defaultLocale}
            key={lang.code}
            //onChange={() => setSelectedLanguage()}
            onSelect={() => setSelectedLanguage(lang.code)}
            className={
              locale === lang.code
                ? "bg-primary text-primary-foreground"
                : ""
            }
          >
            <div className="flex items-center gap-2">
              <img
                src={lang.flag}
                alt={lang.name}
                width={24}
                height={24}
                className="rounded-full"
                style={{ aspectRatio: "24/24", objectFit: "cover" }}
              />
              {lang.name}
            </div>
          </DropdownMenuItem>
        ))}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
