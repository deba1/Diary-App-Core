import axios from 'axios';
import React, { useState } from 'react';
import { Link, useHistory } from 'react-router-dom';
import { rootUrl } from '../hooks/useFetcher';

export default function RegisterPage({ isAdmin = false }) {
    const [request, setRequest] = useState({
        username: "",
        password: "",
        email: ""
    });
    const [error, setError] = useState({
        default: undefined
    });
    const history = useHistory();

    const onInputChange = (key, val) => {
        setRequest({
            ...request,
            [key]: val
        });
    };

    const onRegister = (e) => {
        e.preventDefault();
        axios.post(`${rootUrl}auth/register`, request, { method: "POST" })
            .then(res => {
                alert("Registered Successfully!");
                history.push(isAdmin ? "/user-manager" : "/auth/login")
            })
            .catch(err => {
                console.log(err.response);
                if (err.response?.status === 400) {
                    if (Array.isArray(err.response.data))
                        setError({
                            default: err.response.data[0].description
                        })
                    else {
                        let errors = err.response.data.errors
                        Object.keys(errors).forEach(e => {
                            setError({
                                ...error,
                                [e.toLowerCase()]: errors[e][0]
                            });
                        })
                    }
                }
                else
                    setError(err.message);
            });
    }

    return (
        <div className="LoginPage">
            <form onSubmit={onRegister}>
                <h3 className="SectionTitle">{isAdmin && <Link to="/user-manager" className="btn">&larr;</Link>} New User</h3>
                {error.default && <div className="error">{error.default}</div>}
                <input type="text" placeholder="Username" onChange={(e) => onInputChange("username", e.target.value)} />
                {error.username && <div className="error">{error.username}</div>}
                <input type="email" placeholder="Email" onChange={(e) => onInputChange("email", e.target.value)} />
                {error.email && <div className="error">{error.email}</div>}
                <input type="password" placeholder="Password" onChange={(e) => onInputChange("password", e.target.value)} />
                {error.password && <div className="error">{error.password}</div>}
                <button className="btn">{isAdmin ? "Add" : "Register"}</button>
                {!isAdmin && <Link to="/auth/login" className="btn">Login</Link>}
            </form>
        </div>
    )
}
