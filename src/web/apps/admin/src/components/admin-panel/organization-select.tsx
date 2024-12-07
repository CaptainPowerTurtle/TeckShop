"use client";

import { useLocale } from "next-intl";
import { useTransition } from "react";
import { usePathname, useRouter } from "~/src/navigation";
import { Button } from "@teckshop/ui/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@teckshop/ui/components/ui/dropdown-menu";
import { OrganizationList } from "~/src/schemas/keycloak-schema";
import { useStore } from "@teckshop/ui/hooks/use-store";
import { useTenantToggleStore } from "~/src/hooks/use-tenant-toggle";

export function OrganizationSelect({organizations} : {organizations: OrganizationList}) {
    const router = useRouter();
    const [isPending, startTransition] = useTransition();
    const pathname = usePathname();
    const locale = useLocale();

    const tenantToggle = useStore(useTenantToggleStore, (state) => state);
  
    if(!tenantToggle) return null;
  
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
            {organizations.find((org) => org.id === organizations[0]?.id)?.name || ""}
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end" className="w-[200px]">
          {organizations.map((org) => (
            <DropdownMenuItem
              defaultValue={organizations[0]?.id}
              key={org.id}
              onSelect={() => tenantToggle?.setTenant(org)}
              //onChange={() => setSelectedLanguage()}
              className={
                locale === org.id
                  ? "bg-primary text-primary-foreground"
                  : ""
              }
            >
            <div className="flex items-center gap-2">
              {org.name}
            </div>
            </DropdownMenuItem>
          ))}
        </DropdownMenuContent>
      </DropdownMenu>
    );
  }
  