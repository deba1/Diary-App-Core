import axios from 'axios';
import React, { useCallback, useEffect, useState } from 'react';
import { Link, Redirect } from 'react-router-dom';
import { rootUrl } from '../hooks/useFetcher';
import { useGlobalState } from '../hooks/useGlobalState';

export default function UserManagerPage() {
    const { user } = useGlobalState();
    const [users, setUsers] = useState([]);

    const updateUsers = useCallback(() => {
        if (user) {
            axios.get(`${rootUrl}users`, {
                headers: {
                    Authorization: `Bearer ${user.token}`
                }
            })
                .then(res => {
                    setUsers(res.data.filter(u => u.id !== user.id));
                })
                .catch(err => {
                    console.log(err);
                });
        }
    }, [user]);

    useEffect(() => {
        updateUsers();
    }, [updateUsers]);


    const onDelete = (id) => {
        if (window.confirm("Are you sure?")) {
            axios.delete(`${rootUrl}users/${id}`, {
                headers: { Authorization: `Bearer ${user.token}` }
            })
                .finally(updateUsers);
        }
    }

    return user ? (
        <div style={{ marginTop: "56px", padding: "15px" }}>
            <h4 className="SectionTitle"><Link to="/admin" className="btn">&larr;</Link> User Manager <Link to="/add-user" className="btn">Add New</Link></h4>
            {users.map((u, i) => (
                <div key={i} className="Note">
                    <strong>{u.username}</strong><br />
                    <small>Email: {u.email}</small><br />
                    <small>Roles: {u.roles.join(", ")}</small>
                    <div className="Actions">
                        <button className="btn error" onClick={() => onDelete(u.id)}>Delete</button>
                    </div>
                </div>
            ))}
        </div>
    ) : <Redirect to="/auth/login" />
}
