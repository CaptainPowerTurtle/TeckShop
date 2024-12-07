import { z } from "zod";

export const organizationSchema = z.object({
    id: z.string(),
    name: z.string(),
    alias: z.string(),
    enabled: z.boolean(),
  });
  export type Organization = z.infer<typeof organizationSchema>

  export const organizationListSchema = z.array(organizationSchema);
  export type OrganizationList = z.infer<typeof organizationListSchema>

  export const getUserOrganizationSchema = z.object({
    id: z.string(),
  });
  export type GetUserOrganization = z.infer<typeof getUserOrganizationSchema>