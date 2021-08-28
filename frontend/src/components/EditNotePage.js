import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { Link, Redirect, useHistory, useParams } from 'react-router-dom';
import { rootUrl } from '../hooks/useFetcher';
import { useGlobalState } from '../hooks/useGlobalState';
import Input from './Input';

export default function EditNotePage() {
    const { noteId } = useParams();

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

    useEffect(() => {
        if (user && noteId) {
            axios.get(`${rootUrl}users/me/notes/${noteId}`, {
                headers: { Authorization: `Bearer ${user.token}` }
            })
                .then(res => {
                    setRequest({
                        title: {
                            ...request.title,
                            value: res.data.title
                        },
                        body: {
                            ...request.body,
                            value: res.data.body
                        },
                        date: {
                            ...request.date,
                            value: new Date(res.data.date).toISOString().substr(0, 10)
                        }
                    });
                }).catch(err => {
                    setError({
                        default: err.message
                    });
                });
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [noteId, user]);

    const onUpdate = (e) => {
        e.preventDefault();
        axios.put(`${rootUrl}notes/${noteId}`, {
            title: request.title.value,
            body: request.body.value,
            date: request.date.value
        }, {
            headers: {
                Authorization: `Bearer ${user.token}`
            }
        })
            .then(res => {
                alert("Updated Successfully!");
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
                    setError({ default: "Feature Disabled or You don't have permission" });
                }
                else
                    setError({ default: err.message });
            });
    }

    return user ? (
        <div className="LoginPage">
            <form onSubmit={onUpdate}>
                <h3><Link to="/notes" className="btn">&larr;</Link> Edit Note</h3>
                {error.default && <div className="error">{error.default}</div>}
                {
                    Object.keys(inputItems)
                        .map((inp) => (
                            <Input
                                key={inp}
                                type={inputItems[inp].type}
                                placeholder={inputItems[inp].display}
                                error={error[inp]}
                                defaultValue={request?.[inp]?.value}
                                onChange={(e) => onInputChange(inp, e.target.value)}
                            />
                        ))
                }
                <button className="btn">Update</button>
            </form>
        </div>
    ) : <Redirect to="/auth/login" />;
}
