"use client"

import * as React from "react"
import { zodResolver } from "@hookform/resolvers/zod"

import { toast } from "sonner"

import { Button } from "@teckshop/ui/components/ui/button"
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@teckshop/ui/components/ui/form"
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@teckshop/ui/components/ui/select"
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
} from "@teckshop/ui/components/ui/sheet"
import { Textarea } from "@teckshop/ui/components/ui/textarea"
import { Switch } from "@teckshop/ui/components/ui/switch"
import { useAction } from "next-safe-action/hooks"
import { Spinner } from "@teckshop/ui/components/ui/spinner"
import { useForm } from "react-hook-form"
import { Input } from "@teckshop/ui/components/ui/input"
import { Product, UpdateProduct, updateProductSchema } from "~/src/schemas/product-schema"
import { updateProductAction } from "~/src/app/actions/product-actions"


interface UpdateProductSheetProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  product: Product | null
}

export function UpdateProductSheet({ product, ...props }: UpdateProductSheetProps) {
  const [isUpdatePending, startUpdateTransition] = React.useTransition()

  const { executeAsync, result, status, isPending, hasErrored, hasSucceeded  } = useAction(updateProductAction, {
    onSuccess: ({data}) => {
      toast.success("Product updated", {
        description: "Product have been sucessfully updated",
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
  
  const form = useForm<UpdateProduct>({
    resolver: zodResolver(updateProductSchema),
    defaultValues: {
      name: product?.name,
      description: product?.description ?? "",
      gtin: product?.gtin ?? "",
      productSKU: product?.productSKU ?? "",
      isActive: product?.isActive ?? true,
    },
  })

  React.useEffect(() => {
    form.reset({
      name: product?.name,
      description: product?.description ?? "",
      gtin: product?.gtin ?? "",
      productSKU: product?.productSKU ?? "",
      isActive: product?.isActive ?? true,
    })
  }, [product, form])
  function onSubmit() {
    
    if(!product) return;

    const result = executeAsync({...form.getValues(), id: product.id});

    if (hasErrored) {
      return;
    }
    form.reset()
    props.onOpenChange?.(false)
  }

  return (
    <Sheet {...props}>
      <SheetContent className="flex flex-col gap-6 sm:max-w-md">
        <SheetHeader className="text-left">
          <SheetTitle>Update task</SheetTitle>
          <SheetDescription>
            Update the product details and save the changes
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
                    placeholder="Long description of the product..."
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
              name="gtin"
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
                <FormLabel>GTIN</FormLabel>
                <FormControl>
                  <Input placeholder="1234..." {...field} />
                </FormControl>
                <FormDescription>
                  This is the GTIN number
                </FormDescription>
                <FormMessage />
              </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="productSKU"
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
                <FormLabel>SKU</FormLabel>
                <FormControl>
                  <Input placeholder="1234..." {...field} />
                </FormControl>
                <FormDescription>
                  This is the product SKU
                </FormDescription>
                <FormMessage />
              </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="isActive"
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
                <FormLabel>GTIN</FormLabel>
                <FormControl>
                <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                </FormControl>
                <FormDescription>
                  This is the GTIN number
                </FormDescription>
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