export const useFetcher = async (url, method, body) => {

    return fetch(rootUrl + url, {
        method: method ?? "GET"
    }).then(data => data.json());
}

export const rootUrl = "https://localhost:44352/api/";