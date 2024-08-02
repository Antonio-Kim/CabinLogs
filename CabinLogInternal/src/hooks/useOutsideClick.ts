import { useEffect, useRef } from 'react';

export function useOutsideClick(handler: () => void, listenCapturing = true) {
  const ref = useRef<HTMLDivElement>(null);

  useEffect(() => {
    function handleClick(e: MouseEvent) {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        handler();
      }
    }

    document.addEventListener('click', handleClick, true);

    return () => document.removeEventListener('click', handleClick, true);
  }, [handler, listenCapturing]);

  return ref;
}
