import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useEffect } from 'react';

const CreateReportPage: React.FC = () => {
  const navigate = useNavigate();
  const [message, setMessage] = useState('');

  useEffect(() => {
    axios.get('https://localhost:7178/api/report/generate')
      .then(response => setMessage(response.data.message))
      .catch(error => console.error('Error:', error));
  }, []);
  return <div>{message}</div>;
};

export default CreateReportPage;
