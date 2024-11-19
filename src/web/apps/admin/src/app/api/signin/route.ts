// api/signin/route.ts

import { env } from "~/src/env";
import { signIn } from "~/src/server/auth";




export async function GET(req: Request) {
  const searchParams = new URL(req.url).searchParams;
  return signIn("keycloak", { redirectTo: searchParams.get("callbackUrl") ?? "" });
}