"use client"

import * as React from "react"
import { TrashIcon } from "@radix-ui/react-icons"
import { type Row } from "@tanstack/react-table"
import { toast } from "sonner"

import { useMediaQuery } from "@repo/ui/hooks/use-media-query"
import { Button } from "@repo/ui/components/ui/button"
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@repo/ui/components/ui/dialog"
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@repo/ui/components/ui/drawer"

import { BrandSchema, DeleteBrandsSchema, deleteBrandsSchema } from "~/src/schemas/brand-schema"
import { Spinner } from "@repo/ui/components/ui/spinner"
import { useAction } from "next-safe-action/hooks"
import { z } from "zod"
import { deleteBrandsAction } from "~/src/app/actions/brand-actions"

interface DeleteTasksDialogProps
  extends React.ComponentPropsWithoutRef<typeof Dialog> {
  brands: Row<BrandSchema>["original"][]
  showTrigger?: boolean
  onSuccess?: () => void
}

export function DeleteBrandsDialog({
  brands,
  showTrigger = true,
  onSuccess,
  ...props
}: DeleteTasksDialogProps) {
  const [isDeletePending, startDeleteTransition] = React.useTransition()
  const isDesktop = useMediaQuery("(min-width: 640px)")

  const { executeAsync, result, isPending, hasErrored } = useAction(deleteBrandsAction, {
    onSuccess: ({data}) => {
      toast.success("Brand deleted", {
        description: "Brand have med sucessfully deleted",
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
      brands.map((brand) => console.log(brand.id))
        const result = await executeAsync({ids: brands.map((brand) => brand.id)})
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
              Delete ({brands.length})
            </Button>
          </DialogTrigger>
        ) : null}
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Are you absolutely sure?</DialogTitle>
            <DialogDescription>
              This action cannot be undone. This will permanently delete your{" "}
              <span className="font-medium">{brands.length}</span>
              {brands.length === 1 ? " brand" : " brands"} from our servers.
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
            Delete ({brands.length})
          </Button>
        </DrawerTrigger>
      ) : null}
      <DrawerContent>
        <DrawerHeader>
          <DrawerTitle>Are you absolutely sure?</DrawerTitle>
          <DrawerDescription>
            This action cannot be undone. This will permanently delete your{" "}
            <span className="font-medium">{brands.length}</span>
            {brands.length === 1 ? " brand" : " brands"} from our servers.
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
