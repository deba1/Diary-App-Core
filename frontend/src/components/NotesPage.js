import axios from 'axios';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { Link, Redirect } from 'react-router-dom';
import { rootUrl } from '../hooks/useFetcher';
import { useGlobalState } from '../hooks/useGlobalState';

export default function NotesPage() {
    const { user, features } = useGlobalState();
    const [notes, setNotes] = useState([]);

    const canCreate = useMemo(() => features.find(f => f.id === "CreateNote")?.status === 1, [features]);
    const canEdit = useMemo(() => features.find(f => f.id === "UpdateNote")?.status === 1, [features]);
    const canDelete = useMemo(() => features.find(f => f.id === "DeleteNote")?.status === 1, [features]);
    const canViewTrash = useMemo(() => features.find(f => f.id === "ViewTrashNote")?.status === 1, [features]);

    const updateNotes = useCallback(() => {
        if (user) {
            axios.get(`${rootUrl}users/me/notes`, {
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

    return user ? (
        <div style={{ marginTop: "56px", padding: "15px" }}>
            <h4 className="SectionTitle">My Notes <div>{canViewTrash && <Link to="/trashed-notes" className="btn">Trash</Link>}{canCreate && <Link to="/add-note" className="btn">Add New</Link>}</div></h4>
            {notes.map((note, i) => (
                <div key={i} className="Note">
                    <strong>{note.title}</strong>
                    <div className="Actions">
                        {canEdit && <Link to={`/edit-note/${note.id}`} className="btn">Edit</Link>}
                        {canDelete && <button className="btn error" onClick={() => onDelete(note.id)}>Delete</button>}
                    </div>
                    <p>{note.body}</p>
                    <small>Date: {new Date(note.date).toLocaleDateString()}</small><br />
                    <small>Created At: {new Date(note.createdAt).toLocaleString()}</small>
                </div>
            ))}
        </div>
    ) : <Redirect to="/auth/login" />
}
