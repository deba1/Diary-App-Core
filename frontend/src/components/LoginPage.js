import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { Link, useHistory } from 'react-router-dom';
import { rootUrl } from '../hooks/useFetcher';
import { useGlobalState } from '../hooks/useGlobalState';

export default function LoginPage() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState(null);
    const history = useHistory();

    const { user, setUser } = useGlobalState();

    const onLogin = (e) => {
        e.preventDefault();
        axios.post(`${rootUrl}auth/login`, {
            username,
            password
        }, { method: "POST" })
            .then(res => {
                setUser({
                    username,
                    ...res.data
                });
            })
            .catch(err => {
                if (err.response?.status === 400) {
                    setError("Invalid Input");
                }
                else if (err.response?.status === 401) {
                    setError("Username or Password doesn't match");
                }
                else
                    setError(err.message);
            });
    }

    useEffect(() => {
        if (user) {
            if (user.roles.includes("Admin"))
                history.push("/admin");
            else
                history.push("/notes");
        }
    }, [history, user]);

    return (
        <div className="LoginPage">
            <form onSubmit={onLogin}>
                {error && <div className="error">{error}</div>}
                <input type="text" placeholder="Username" onChange={(e) => setUsername(e.target.value)} />
                <input type="password" placeholder="Password" onChange={(e) => setPassword(e.target.value)} />
                <button className="btn">Login</button>
                <Link to="/auth/register" className="btn">Register</Link>
            </form>
        </div>
    )
}
