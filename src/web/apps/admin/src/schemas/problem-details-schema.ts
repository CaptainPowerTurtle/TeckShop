import { z } from "zod"

export const errorSchema = z.object({
    name: z.string(),
    reason: z.string(),
  })
  
  export type Errors = z.infer<typeof errorSchema>
  
  export const problemDetailsSchema = z.object({
    type: z.string(),
    title: z.string(),
    status: z.number(),
    instance: z.string(),
    traceId: z.string(),
    errors: z.array(errorSchema)
  })
  
  export type ProblemDetails = z.infer<typeof problemDetailsSchema>