import React from 'react';

export default function Input({ type = "text", defaultValue = "", error, ...rest }) {
    return <div>
        {(type === "textarea") ?
            <textarea className="Textarea" defaultValue={defaultValue} {...rest} /> :
            <input className="Input" type={type} defaultValue={defaultValue} {...rest} />
        }
        {error && <div className="error">{error}</div>}
    </div>;
}
