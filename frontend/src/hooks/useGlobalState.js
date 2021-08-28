import axios from 'axios';
import { createContext, useCallback, useContext, useEffect, useState } from 'react';
import { rootUrl } from './useFetcher';

const globalContext = createContext();

export function useGlobalState() {
    return useContext(globalContext);
}

export function GlobalProvider({ children }) {
    const [user, setUser] = useState(null);
    const [features, setFeatures] = useState([]);

    const updateFeatures = useCallback(
        async () => {
            if (user) {
                const response = await axios.get(`${rootUrl}settings`, { headers: { Authorization: `Bearer ${user.token}` } });
                setFeatures(response.data);
            }
        },
        [user],
    );

    useEffect(() => {
        updateFeatures();
    }, [updateFeatures]);


    return <globalContext.Provider value={{
        user, setUser,
        features, updateFeatures
    }}>
        {children}
    </globalContext.Provider>
}