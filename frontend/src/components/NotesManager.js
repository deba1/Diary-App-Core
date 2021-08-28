import axios from 'axios';
import React, { useCallback, useEffect, useState } from 'react';
import { Link, Redirect } from 'react-router-dom';
import { rootUrl } from '../hooks/useFetcher';
import { useGlobalState } from '../hooks/useGlobalState';

export default function NotesManagerPage() {
    const { user } = useGlobalState();
    const [notes, setNotes] = useState([]);

    const updateNotes = useCallback(() => {
        if (user) {
            axios.get(`${rootUrl}notes`, {
                headers: {
                    Authorization: `Bearer ${user.token}`
                }
            })
                .then(res => {
                    setNotes(res.data);
                })
                .catch(err => {
                    console.log(err);
                });
        }
    }, [user]);

    useEffect(() => {
        updateNotes();
    }, [updateNotes]);

    const onDelete = (id) => {
        if (window.confirm("Are you sure?")) {
            axios.delete(`${rootUrl}notes/${id}`, {
                headers: { Authorization: `Bearer ${user.token}` }
            })
                .finally(updateNotes);
        }
    }

    return user && user.roles.includes("Admin") ? (
        <div style={{ marginTop: "56px", padding: "15px" }}>
            <h4 className="SectionTitle">All Notes <Link to="/add-note" className="btn">Add New</Link></h4>
            {notes.map((note, i) => (
                <div key={i} className="Note">
                    <strong>{note.title}</strong>
                    <div className="Actions">
                        <button className="btn error" onClick={() => onDelete(note.id)}>Delete</button>
                    </div>
                    <p>{note.body}</p>
                    <small>Date: {new Date(note.date).toLocaleDateString()}</small><br />
                    <small>Created At: {new Date(note.createdAt).toLocaleString()}</small>
                </div>
            ))}
        </div>
    ) : <Redirect to="/auth/login" />
}
