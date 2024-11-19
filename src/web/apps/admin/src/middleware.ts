
import createMiddleware from 'next-intl/middleware';
import { NextRequest, NextResponse } from 'next/server';
import { localeCodes } from './config/i18n/config';
import { auth } from './server/auth';
import {env} from "./env"

const publicPages = [
  '/',
];
const protectedPages = [
  '/dashboard/.*',
  '/brands/.*',
  // (/secret requires auth)
];

const authMiddleware = auth((req) => {
  if (req.auth) return intlMiddleware(req);
  
  const reqUrl = new URL(req.url);
  if (!req.auth && reqUrl?.pathname !== "/") {
    return NextResponse.redirect(
      new URL(`/api/signin?callbackUrl=${encodeURIComponent(reqUrl.pathname)}`, req.url)
    );
  }
}
);
export default function middleware(req: NextRequest) {
  const publicPathnameRegex = RegExp(
    `^(/(${localeCodes.join('|')}))?(${publicPages
      .flatMap((p) => (p === '/' ? ['', '/'] : p))
      .join('|')})/?$`,
    'i'
  );
  const isPublicPage = publicPathnameRegex.test(req.nextUrl.pathname);
  if (isPublicPage) {
    return intlMiddleware(req);
  } else {
    return (authMiddleware as any)(req);
  }
}
const intlMiddleware =  createMiddleware({
  // A list of all locales that are supported
  locales: localeCodes,
 
  // Used when no locale matches
  defaultLocale: 'en',
});

export const config = {
  matcher: ['/((?!api|_next|.*\\..*).*)']
}