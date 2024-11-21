"use client"
import { DownloadIcon } from "@radix-ui/react-icons"


import { exportTableToCSV } from "@repo/ui/lib/export"
import { Button } from "@repo/ui/components/ui/button"

//import { CreateTaskDialog } from "./create-task-dialog"

import { Table } from "@tanstack/react-table"
import { DeleteProductsDialog } from "./delete-products-dialog"
import { Product } from "~/src/schemas/product-schema"

interface ProductsTableToolbarActionsProps {
  table: Table<Product>
}

export function ProductsTableToolbarActions({
  table,
}: ProductsTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      {table.getFilteredSelectedRowModel().rows.length > 0 ? (
        <DeleteProductsDialog
          products={table
            .getFilteredSelectedRowModel()
            .rows.map((row) => row.original)}
          onSuccess={() => table.toggleAllRowsSelected(false)}
        />
      ) : null}
      {/* <CreateTaskDialog /> */}
      <Button
        variant="outline"
        size="sm"
        onClick={() =>
          exportTableToCSV(table, {
            filename: "tasks",
            excludeColumns: ["select", "actions"],
          })
        }
      >
        <DownloadIcon className="mr-2 size-4" aria-hidden="true" />
        Export
      </Button>
      {/**
       * Other actions can be added here.
       * For example, import, view, etc.
       */}
    </div>
  )
}
