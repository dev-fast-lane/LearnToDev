import {useCallback, useState, type ReactNode} from "react";
import {ToastContext, type ToastType} from "./toastContext";

type Toast = {
    id: number;
    message: string;
    type: ToastType;
};
// ToastProvider component that provides the showToast function to its children and manages the display of toast notifications.
export default function ToastProvider({ children }: { children: ReactNode }) {
    const [toasts, setToasts] = useState<Toast[]>([]);

    const showToast = useCallback((message: string, type: ToastType = "success") => {
        const id = Date.now() + Math.random();
        setToasts((prevToasts) => [...prevToasts, { id, message, type }]);
        setTimeout(() => {
            setToasts((prevToasts) => prevToasts.filter((toast) => toast.id !== id));
        }, 4000);
    }, []);

    return (
        <ToastContext value={showToast}>
            {children}
            <div className="toasts" aria-live="polite">
                {toasts.map((toast) => (
                    <div key={toast.id} className={`toast toast-${toast.type}`}>
                        {toast.message}
                    </div>
                ))}
            </div>
        </ToastContext>
    );
}