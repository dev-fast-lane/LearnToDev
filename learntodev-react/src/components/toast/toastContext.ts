import { createContext } from "react";

export type ToastType = "success" | "error";
export type ShowToast = (message: string, type?: ToastType) => void;

export const ToastContext = createContext<ShowToast | null>(null);