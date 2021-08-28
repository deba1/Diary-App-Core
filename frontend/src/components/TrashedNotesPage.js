import axios from 'axios';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { Redirect } from 'react-router-dom';
import { rootUrl } from '../hooks/useFetcher';
import { useGlobalState } from '../hooks/useGlobalState';

export default function TrashedNotesPage() {
    const { user, features } = useGlobalState();
    const [notes, setNotes] = useState([]);

    const canViewTrash = useMemo(() => features.find(f => f.id === "ViewTrashNote")?.status === 1, [features]);
    const carRecover = useMemo(() => features.find(f => f.id === "RecoverNote")?.status === 1, [features]);

    const updateNotes = useCallback(() => {
        if (user && canViewTrash) {
            axios.get(`${rootUrl}trashes/notes`, {
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
    }, [canViewTrash, user]);

    useEffect(() => {
        updateNotes();
    }, [updateNotes]);

    const onRecover = (id) => {
        axios.patch(`${rootUrl}trashes/notes/${id}`, {}, {
            headers: { Authorization: `Bearer ${user.token}` }
        })
            .finally(updateNotes);
    }

    return user ? (
        <div style={{ marginTop: "56px", padding: "15px" }}>
            <h4 className="SectionTitle">Trashed Notes</h4>
            {notes.map((note, i) => (
                <div key={i} className="Note">
                    <strong>{note.title}</strong>
                    <div className="Actions">
                        {carRecover && <button className="btn" onClick={() => onRecover(note.id)}>Recover</button>}
                    </div>
                    <p>{note.body}</p>
                    <small>Date: {new Date(note.date).toLocaleDateString()}</small><br />
                    <small>Created At: {new Date(note.createdAt).toLocaleString()}</small>
                </div>
            ))}
        </div>
    ) : <Redirect to="/auth/login" />
}
