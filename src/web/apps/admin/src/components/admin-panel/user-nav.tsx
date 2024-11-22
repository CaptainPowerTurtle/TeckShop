"use client";


import { LayoutGrid, LogOut, User } from "lucide-react";


import React from "react";


import { Avatar, AvatarImage, AvatarFallback } from "@teckshop/ui/components/ui/avatar";
import { Button } from "@teckshop/ui/components/ui/button";
import { TooltipProvider, Tooltip, TooltipTrigger, TooltipContent } from "@teckshop/ui/components/ui/tooltip";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger } from "@teckshop/ui/components/ui/dropdown-menu";
import { signOut } from "next-auth/react";
import { User as AuthUser } from "next-auth";
import { Link } from "~/src/navigation";

interface UserNavProps {
  profile: string;
  dashboard: string;
  account: string;
  signOut: string;
}

// interface User {
//   name: string | null | undefined;
//   email: string | null | undefined;
// }

export function UserNav({userNavProps, user} : {userNavProps: UserNavProps, user: AuthUser}) {


  return (
    <DropdownMenu>
      <TooltipProvider disableHoverableContent>
        <Tooltip delayDuration={100}>
          <TooltipTrigger asChild>
            <DropdownMenuTrigger asChild>
              <Button
                variant="outline"
                className="relative h-8 w-8 rounded-full"
              >
                <Avatar className="h-8 w-8">
                  <AvatarImage src="#" alt="Avatar" />
                  <AvatarFallback className="bg-transparent"><User size={18}/></AvatarFallback>
                </Avatar>
              </Button>
            </DropdownMenuTrigger>
          </TooltipTrigger>
          <TooltipContent side="bottom">{userNavProps.profile}</TooltipContent>
        </Tooltip>
      </TooltipProvider>

      <DropdownMenuContent className="w-56" align="end" forceMount>
        <DropdownMenuLabel className="font-normal">
          <div className="flex flex-col space-y-1">
            <p className="text-sm font-medium leading-none">{user.name}</p>
            <p className="text-xs leading-none text-muted-foreground">
              {user.email}
            </p>
          </div>
        </DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuGroup>
          <DropdownMenuItem className="hover:cursor-pointer" asChild>
            <Link href="/dashboard" className="flex items-center">
              <LayoutGrid className="w-4 h-4 mr-3 text-muted-foreground" />
              {userNavProps.dashboard}
            </Link>
          </DropdownMenuItem>
          <DropdownMenuItem className="hover:cursor-pointer" asChild>
            <Link href="/account" className="flex items-center">
              <User className="w-4 h-4 mr-3 text-muted-foreground" />
              {userNavProps.account}
            </Link>
          </DropdownMenuItem>
        </DropdownMenuGroup>
        <DropdownMenuSeparator />
        <DropdownMenuItem className="hover:cursor-pointer" onClick={() => signOut({ callbackUrl: "/", redirect: true })}>
          <LogOut className="w-4 h-4 mr-3 text-muted-foreground" />
          {userNavProps.signOut}
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}