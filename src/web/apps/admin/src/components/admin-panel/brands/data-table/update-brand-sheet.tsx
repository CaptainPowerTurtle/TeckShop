"use client"

import * as React from "react"
import { zodResolver } from "@hookform/resolvers/zod"

import { toast } from "sonner"

import { Button } from "@repo/ui/components/ui/button"
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@repo/ui/components/ui/form"
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@repo/ui/components/ui/select"
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
} from "@repo/ui/components/ui/sheet"
import { Textarea } from "@repo/ui/components/ui/textarea"


import { BrandSchema, updateBrandSchema, type UpdateBrandSchema } from "~/src/schemas/brand-schema"
import { useAction } from "next-safe-action/hooks"
import { updateBrandAction } from "~/src/app/actions/brand-actions"
import { Spinner } from "@repo/ui/components/ui/spinner"
import { useForm } from "react-hook-form"
import { Input } from "@repo/ui/components/ui/input"


interface UpdateBrandSheetProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  brand: BrandSchema | null
}

export function UpdateBrandSheet({ brand, ...props }: UpdateBrandSheetProps) {
  const [isUpdatePending, startUpdateTransition] = React.useTransition()

  const { executeAsync, result, status, isPending, hasErrored, hasSucceeded  } = useAction(updateBrandAction, {
    onSuccess: ({data}) => {
      toast.success("Brand updated", {
        description: "Brand have been sucessfully updated",
        richColors: true,
        closeButton: true,
      })
    },
    onError: ({error}) => {
      if (error.fetchError || error.serverError) {
        toast.error("Something went wrong", {
          description: error.serverError != null ? error.serverError : error.fetchError,
          richColors: true,
          duration: Infinity,
        })
      }else if(error.validationErrors){
        toast.error("Validation error!", {
          description: 'One or more validations failed!',
          richColors: true,
          duration: Infinity,
        })
      }
    }
  })
  
  const form = useForm<UpdateBrandSchema>({
    resolver: zodResolver(updateBrandSchema),
    defaultValues: {
        name: brand?.name,
        description: brand?.description ?? "",
        website: brand?.website ?? ""
    },
  })

  React.useEffect(() => {
    form.reset({
        name: brand?.name,
        description: brand?.description ?? "",
        website: brand?.website ?? "",
    })
  }, [brand, form])
  function onSubmit() {
    
    if(!brand) return;

    const result = executeAsync({...form.getValues(), id: brand.id});

    if (hasErrored) {
      return;
    }
    form.reset()
    props.onOpenChange?.(false)
    // startUpdateTransition(async () => {
    //     const result = await executeAsync({...input, id: brand.id,});

    //   if (result?.serverError || result?.validationErrors) {
    //     return
    //   }
    //   status
    //   form.reset()
    //   props.onOpenChange?.(false)
    //   toast.success("Brand updated")
    // })
  }

  return (
    <Sheet {...props}>
      <SheetContent className="flex flex-col gap-6 sm:max-w-md">
        <SheetHeader className="text-left">
          <SheetTitle>Update task</SheetTitle>
          <SheetDescription>
            Update the brand details and save the changes
          </SheetDescription>
        </SheetHeader>
        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(onSubmit)}
            className="flex flex-col gap-4"
          >
            <FormField
              control={form.control}
              name="name"
              render={({ field }) => (
                // <FormItem>
                //   <FormLabel>Name</FormLabel>
                //   <FormControl>
                //     <Textarea
                //       placeholder="Do a kickflip"
                //       className="resize-none"
                //       {...field}
                //     />
                //   </FormControl>
                //   <FormMessage />
                // </FormItem>
                <FormItem>
                <FormLabel>Name</FormLabel>
                <FormControl>
                  <Input placeholder="shadcn" {...field} />
                </FormControl>
                <FormDescription>
                  This is your public display name.
                </FormDescription>
                <FormMessage />
              </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="description"
              render={({ field }) => (
                // <FormItem>
                //   <FormLabel>Description</FormLabel>
                //   <FormControl>
                //     <Textarea
                //       placeholder="Do a kickflip"
                //       {...field}
                //     />
                //   </FormControl>
                //   <FormMessage />
                // </FormItem>
                <FormItem>
                <FormLabel>Description</FormLabel>
                <FormControl>
                  <Textarea
                    placeholder="Long description of the brand..."
                    {...field}
                  />
                </FormControl>
                <FormDescription>
                  This is your public display name.
                </FormDescription>
                <FormMessage />
              </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="website"
              render={({ field }) => (
                // <FormItem>
                //   <FormLabel>Website</FormLabel>
                //   <FormControl>
                //     <Textarea
                //       placeholder="Do a kickflip"
                //       className="resize-none"
                //       {...field}
                //     />
                //   </FormControl>
                //   <FormMessage />
                // </FormItem>
                <FormItem>
                <FormLabel>Website</FormLabel>
                <FormControl>
                  <Input placeholder="https://teck.dk" {...field} />
                </FormControl>
                <FormDescription>The Website of the brand</FormDescription>
                <FormMessage />
              </FormItem>
              )}
            />
            <SheetFooter className="gap-2 pt-2 sm:space-x-0">
              <SheetClose asChild>
                <Button type="button" variant="outline">
                  Cancel
                </Button>
              </SheetClose>
              <Button type='submit'>
                {isPending && (
                  <Spinner
                    className="mr-2 size-4 animate-spin"
                    aria-hidden="true"
                  />
                )}
                Save
              </Button>
            </SheetFooter>
          </form>
        </Form>
      </SheetContent>
    </Sheet>
  )
}