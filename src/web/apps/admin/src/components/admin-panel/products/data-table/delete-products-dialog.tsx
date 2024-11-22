"use client"

import * as React from "react"
import { TrashIcon } from "@radix-ui/react-icons"
import { type Row } from "@tanstack/react-table"
import { toast } from "sonner"

import { useMediaQuery } from "@teckshop/ui/hooks/use-media-query"
import { Button } from "@teckshop/ui/components/ui/button"
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@teckshop/ui/components/ui/dialog"
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@teckshop/ui/components/ui/drawer"

import { Spinner } from "@teckshop/ui/components/ui/spinner"
import { useAction } from "next-safe-action/hooks"
import { z } from "zod"
import { Product } from "~/src/schemas/product-schema"
import { deleteProductsAction } from "~/src/app/actions/product-actions"

interface DeleteTasksDialogProps
  extends React.ComponentPropsWithoutRef<typeof Dialog> {
  products: Row<Product>["original"][]
  showTrigger?: boolean
  onSuccess?: () => void
}

export function DeleteProductsDialog({
  products,
  showTrigger = true,
  onSuccess,
  ...props
}: DeleteTasksDialogProps) {
  const [isDeletePending, startDeleteTransition] = React.useTransition()
  const isDesktop = useMediaQuery("(min-width: 640px)")

  const { executeAsync, result, isPending, hasErrored, } = useAction(deleteProductsAction, {
    onSuccess: ({data}) => {
      toast.success("Products deleted", {
        description: "Product was sucessfully deleted",
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
          description: 'One or more validation failed!',
          richColors: true,
          duration: Infinity,
        })
      }
    }
  })


  function onDelete() {
    startDeleteTransition(async () => {
        const result = await executeAsync({ids: products.map((product) => product.id)})
      if (hasErrored) {
        //toast.error(error)
        return
      }

      props.onOpenChange?.(false)
      onSuccess?.()
    })
  }

  if (isDesktop) {
    return (
      <Dialog {...props}>
        {showTrigger ? (
          <DialogTrigger asChild>
            <Button variant="outline" size="sm">
              <TrashIcon className="mr-2 size-4" aria-hidden="true" />
              Delete ({products.length})
            </Button>
          </DialogTrigger>
        ) : null}
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Are you absolutely sure?</DialogTitle>
            <DialogDescription>
              This action cannot be undone. This will permanently delete your{" "}
              <span className="font-medium">{products.length}</span>
              {products.length === 1 ? " product" : " products"}.
            </DialogDescription>
          </DialogHeader>
          <DialogFooter className="gap-2 sm:space-x-0">
            <DialogClose asChild>
              <Button variant="outline">Cancel</Button>
            </DialogClose>
            <Button
              aria-label="Delete selected rows"
              variant="destructive"
              onClick={onDelete}
              disabled={isDeletePending}
            >
              {isDeletePending && (
                <Spinner
                //   className="mr-2 size-4 animate-spin"
                //   aria-hidden="true"
                />
              )}
              Delete
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    )
  }

  return (
    <Drawer {...props}>
      {showTrigger ? (
        <DrawerTrigger asChild>
          <Button variant="outline" size="sm">
            <TrashIcon className="mr-2 size-4" aria-hidden="true" />
            Delete ({products.length})
          </Button>
        </DrawerTrigger>
      ) : null}
      <DrawerContent>
        <DrawerHeader>
          <DrawerTitle>Are you absolutely sure?</DrawerTitle>
          <DrawerDescription>
            This action cannot be undone. This will permanently delete your{" "}
            <span className="font-medium">{products.length}</span>
            {products.length === 1 ? " product" : " products"}.
          </DrawerDescription>
        </DrawerHeader>
        <DrawerFooter className="gap-2 sm:space-x-0">
          <DrawerClose asChild>
            <Button variant="outline">Cancel</Button>
          </DrawerClose>
          <Button
            aria-label="Delete selected rows"
            variant="destructive"
            onClick={onDelete}
            disabled={isDeletePending}
          >
            {isDeletePending && (
              <Spinner
                // className="mr-2 size-4 animate-spin"
                // aria-hidden="true"
              />
            )}
            Delete
          </Button>
        </DrawerFooter>
      </DrawerContent>
    </Drawer>
  )
}
