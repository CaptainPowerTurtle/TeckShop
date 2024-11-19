import { UserNav } from "./user-nav";
import { SheetMenu } from "./sheet-menu";
import { ModeToggle } from "@repo/ui/components/mode-toggle";
import { Button } from "@repo/ui/components/ui/button";
import { getTranslations } from "next-intl/server";
import { auth, signIn } from "~/src/server/auth";
import { LanguageSelect } from "./language-select";

interface NavbarProps {
  title: string;
}

export async function Navbar({ title }: NavbarProps) {
  const session = await auth()
  const t = await getTranslations("rootlayout");
  session?.user.provider
  return (
    <header className="sticky top-0 z-10 w-full bg-background/95 shadow backdrop-blur supports-[backdrop-filter]:bg-background/60 dark:shadow-secondary">
      <div className="mx-4 sm:mx-8 flex h-14 items-center">
        <div className="flex items-center space-x-4 lg:space-x-0">
          <SheetMenu />
          <h1 className="font-bold">{title}</h1>
        </div>
        <div className="flex flex-1 items-center space-x-2 justify-end">
          <LanguageSelect />
          <ModeToggle />
          {session?.user != null ? (
            <UserNav
              user={session.user}
              userNavProps={{
                dashboard: t('dashboard'),
                account: t('account'),
                signOut: t('signOut'),
                profile: t('profile')
              }}
            />
          ) : (
            <Button title="Sign In" />
          )}
        </div>
      </div>
    </header>
  );
}
