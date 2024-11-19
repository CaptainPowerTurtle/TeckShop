"use client"
import { DownloadIcon } from "@radix-ui/react-icons"


import { exportTableToCSV } from "@repo/ui/lib/export"
import { Button } from "@repo/ui/components/ui/button"

//import { CreateTaskDialog } from "./create-task-dialog"

import { BrandSchema } from "~/src/schemas/brand-schema"
import { DeleteBrandsDialog } from "./delete-brands-dialog"
import { Table } from "@tanstack/react-table"

interface TasksTableToolbarActionsProps {
  table: Table<BrandSchema>
}

export function BrandsTableToolbarActions({
  table,
}: TasksTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      {table.getFilteredSelectedRowModel().rows.length > 0 ? (
        <DeleteBrandsDialog
          brands={table
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
