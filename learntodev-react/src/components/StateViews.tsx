export function Loading({ text = "Laster..." }: { text?: string }) {
    return <p role="status" className="state state-loading">{text}</p>;
}

export function ErrorMessage({ message, onRetry }: { message: string; onRetry?: () => void }) {
    return (
        <div role="alert" className="state state-error">
            <p>{message}</p>
            {onRetry && (
                <button className="btn btn-secondary" onClick={onRetry}>
                    Prøv igjen
                </button>
            )}
        </div>
    );
}

export function EmptyState({ text }: { text: string }) {
    return <p className="state state-empty">{text}</p>;
}