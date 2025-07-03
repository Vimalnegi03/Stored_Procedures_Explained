import React, { useState, useEffect } from 'react';
import ReactFileReader from 'react-file-reader';
import './HomePage.css';
import { Button, Pagination } from '@mui/material';

function HomePage() {
  const [records, setRecords] = useState([]);
  const [fileName, setFileName] = useState('');
  const [page, setPage] = useState(1); // 1-based UI
  const [totalCount, setTotalCount] = useState(0);

  const pageSize = 5; // Based on your request payload
  const totalPages = Math.ceil(totalCount / pageSize);

  useEffect(() => {
    fetchRecords(page - 1); // Send 0-based page to backend
  }, [page]);

  const fetchRecords = (zeroBasedPageNumber) => {
    fetch('http://localhost:5256/api/UploadFile/ReadRecord', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        pageNumber: zeroBasedPageNumber,
        numberOfRecordPerPage: pageSize,
      }),
    })
      .then((res) => res.json())
      .then((data) => {
        console.log('API Response:', data); // Debug log
        setRecords(data.data || []);
        setTotalCount(data.totalCount || 0);
      })
      .catch((err) => console.error('Error fetching records:', err));
  };
console.log(records)
  const handleFileUpload = (files) => {
    const file = files[0];
    const formData = new FormData();
    formData.append('file', file);
    setFileName(file.name);

    const isCsv = file.name.endsWith('.csv');
    const isXlsx = file.name.endsWith('.xlsx');

    if (!isCsv && !isXlsx) {
      alert('Only .csv or .xlsx files are allowed');
      return;
    }

    const uploadUrl = isCsv
      ? 'http://localhost:5256/api/UploadFile/UploadCsvFile'
      : 'http://localhost:5256/api/UploadFile/UploadExcelFile';

    fetch(uploadUrl, {
      method: 'POST',
      body: formData,
    })
      .then((res) => res.json())
      .then(() => {
        alert('File uploaded successfully');
        fetchRecords(page - 1); // refresh list
      })
      .catch((err) => console.error('Upload failed:', err));
  };

  const handleDelete = (id) => {
    fetch(`http://localhost:5256/api/UploadFile/DeleteRecord`, {
      method: 'POST',
       headers: {
      'Content-Type': 'application/json',   // ← add this
    },
      body: JSON.stringify({
        userId:id
      }),
    })
      .then(() => {
        alert('Record deleted');
        fetchRecords(page - 1);
      })
      .catch((err) => console.error('Delete failed:', err));
  };

  const handlePageChange = (event, value) => {
    setPage(value);
  };

  return (
    <div className='Container'>
      <div className='SubContainer'>
        <div className='Box1'>Data Dashboard</div>

        <div className='Box2'>
          <div className='data-flex header-row'>
            <div className='UserID'>UserID</div>
            <div className='UserName'>UserName</div>
            <div className='EmailID'>EmailID</div>
            <div className='MobileNumber'>MobileNumber</div>
            <div className='Age'>Age</div>
            <div className='Salary'>Salary</div>
            <div className='Delete'>Action</div>
          </div>

          {records.map((record) => (
            <div className='data-flex' key={record.UserId}>
              <div>{record.userId}</div>
              <div>{record.userName}</div>
              <div>{record.emailID}</div>
              <div>{record.mobileNumber}</div>
              <div>{record.age}</div>
              <div>{record.salary}</div>
              <div>
                <Button
                  variant='outlined'
                  color='error'
                  size='small'
                  onClick={() => handleDelete(record.userId)}
                >
                  Delete
                </Button>
              </div>
            </div>
          ))}
        </div>

        <div className='Input-Container'>
          <div className='Flex-Container'>
            <div className='Header'>Excel and CSV Bulk Data Uploader</div>
            <div className='Sub-flex-Container'>
              <div className='FileName'>{fileName || 'No file selected'}</div>
              <div className='UploadButton'>
                <ReactFileReader
                  fileTypes={['.csv', '.xlsx']}
                  handleFiles={handleFileUpload}
                >
                  <Button variant='contained' color='primary'>
                    Submit
                  </Button>
                </ReactFileReader>
              </div>
            </div>
          </div>

          <div className='flex-Button'>
            <Pagination
              count={totalPages}
              page={page}
              onChange={handlePageChange}
              variant='outlined'
              shape='rounded'
              color='primary'
            />
          </div>
        </div>
      </div>
    </div>
  );
}

export default HomePage;
