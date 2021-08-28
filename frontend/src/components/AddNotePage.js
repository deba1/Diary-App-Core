import axios from 'axios';
import React, { useState } from 'react';
import { Link, Redirect, useHistory } from 'react-router-dom';
import { rootUrl } from '../hooks/useFetcher';
import { useGlobalState } from '../hooks/useGlobalState';
import Input from './Input';

export default function AddNotePage() {
    const inputItems = {
        title: {
            display: "Title",
            type: "text",
            value: ""
        },
        body: {
            display: "Body",
            type: "textarea",
            value: ""
        },
        date: {
            display: "Date",
            type: "date",
            value: `${new Date().toISOString().substr(0, 10)}`
        }
    };
    const [request, setRequest] = useState(inputItems);

    const [error, setError] = useState({
        default: undefined
    });

    const { user } = useGlobalState();
    const history = useHistory();

    const onInputChange = (key, val) => {
        setRequest({
            ...request,
            [key]: {
                value: val
            }
        });
    };

    const onCreate = (e) => {
        e.preventDefault();
        axios.post(`${rootUrl}notes`, {
            title: request.title.value,
            body: request.body.value,
            date: request.date.value
        }, {
            method: "POST",
            headers: {
                Authorization: `Bearer ${user.token}`
            }
        })
            .then(res => {
                alert("Created Successfully!");
                history.push("/notes")
            })
            .catch(err => {
                if (err.response?.status === 400) {
                    let errors = err.response.data.errors
                    Object.keys(errors).forEach(e => {
                        setError({
                            ...error,
                            [e.toLowerCase()]: errors[e][0]
                        });
                    })
                }
                else if (err.response?.status === 403) {
                    setError({ default: "Feature Disabled" });
                }
                else
                    setError({ default: err.message });
            });
    }

    return user ? (
        <div className="LoginPage">
            <form onSubmit={onCreate}>
                <h3><Link to="/notes" className="btn">&larr;</Link> Add Note</h3>
                {error.default && <div className="error">{error.default}</div>}
                {
                    Object.keys(inputItems)
                        .map((inp) => (
                            <Input
                                key={inp}
                                type={inputItems[inp].type}
                                placeholder={inputItems[inp].display}
                                error={error[inp]}
                                defaultValue={inputItems[inp].value}
                                onChange={(e) => onInputChange(inp, e.target.value)}
                            />
                        ))
                }
                <button className="btn">Create</button>
            </form>
        </div>
    ) : <Redirect to="/auth/login" />;
}
